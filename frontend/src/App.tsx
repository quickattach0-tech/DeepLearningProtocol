import { useEffect, useMemo, useRef, useState } from 'react';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

type Message = {
  id: string;
  sender: 'User' | 'DLP' | 'Analyzer' | 'System' | 'InstructionReader';
  text: string;
  timestamp: string;
};

type Tab = 'chat' | 'faq' | 'privacy';

const FAQ_ENTRIES = [
  {
    question: 'What is the Deep Learning Protocol?',
    answer:
      'A hierarchical reasoning system that processes queries through multiple layers: AbstractCore, Aim interface, Depth interface, State interface, and Data Loss Prevention (DLP).',
  },
  {
    question: 'How do I run the program?',
    answer:
      'Run the backend via `dotnet run` and use the React frontend to chat with agents. Use the upload section to process images and extract instructions via OCR.',
  },
  {
    question: 'What is Data Loss Prevention (DLP)?',
    answer:
      'DLP is a module that scans conversations and data for sensitive or restricted content, and helps enforce privacy and compliance policies.',
  },
  {
    question: 'How do I customize the protocol?',
    answer:
      'Modify the translation rules, add new agents, or adjust processing depth via the core modules in the DeepLearningProtocol project.',
  },
  {
    question: 'How can I use the image upload feature?',
    answer:
      'Upload an instruction image and the system will perform OCR to extract text and add it into the shared conference chat.',
  },
];

const PRIVACY_TEXT = `We follow data protection guidance from the project wiki and documentation.

• We only store data temporarily for conference sessions.
• Uploaded images and extracted text are not persistently stored by default.
• The system is designed to support GDPR-style DLP (Data Loss Prevention) by flagging sensitive content.

For full details, refer to the project documentation and DLP guide in the repository.`;

function formatTimestamp(date = new Date()) {
  return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
}

export default function App() {
  const [tab, setTab] = useState<Tab>('chat');
  const [input, setInput] = useState('');
  const [rating, setRating] = useState('3');
  const [messages, setMessages] = useState<Message[]>([]);
  const [connection, setConnection] = useState<HubConnection | null>(null);
  const chatRef = useRef<HTMLDivElement | null>(null);

  const addMessage = (msg: Message) => {
    setMessages((prev) => [...prev, msg]);
  };

  const userMessage = (text: string) =>
    addMessage({
      id: crypto.randomUUID(),
      sender: 'User',
      text,
      timestamp: formatTimestamp(),
    });

  const systemMessage = (text: string) =>
    addMessage({
      id: crypto.randomUUID(),
      sender: 'System',
      text,
      timestamp: formatTimestamp(),
    });

  const respondFromWiki = (text: string) => {
    const lower = text.toLowerCase();
    if (lower.includes('faq') || lower.includes('question')) {
      const entry = FAQ_ENTRIES[0];
      return `FAQ: ${entry.question}\n\n${entry.answer}`;
    }

    for (const entry of FAQ_ENTRIES) {
      if (lower.includes(entry.question.toLowerCase().split(' ')[0])) {
        return `${entry.question}\n\n${entry.answer}`;
      }
    }

    return `I didn't find a precise FAQ match, but you can browse the FAQ tab for common questions.`;
  };

  const sendMessage = async (text: string) => {
    if (!connection) return;
    userMessage(text);

    // If message resembles a question, provide a wiki-based answer.
    const response = respondFromWiki(text);
    addMessage({
      id: crypto.randomUUID(),
      sender: 'DLP',
      text: response,
      timestamp: formatTimestamp(),
    });

    // Also send to backend for logging (optional)
    try {
      await connection.invoke('SendMessage', 'WebUI', text);
    } catch {
      // ignore
    }
  };

  const sendRating = async (ratingValue: string) => {
    if (!connection) return;
    addMessage({
      id: crypto.randomUUID(),
      sender: 'User',
      text: `Rated content: ${ratingValue} / 5`,
      timestamp: formatTimestamp(),
    });

    try {
      await connection.invoke('SendRating', 'WebUI', ratingValue);
    } catch {
      // ignore
    }
  };

  const processImage = async (file: File) => {
    const form = new FormData();
    form.append('imageFile', file);

    try {
      const res = await fetch('/?handler=UploadImage', {
        method: 'POST',
        body: form,
      });

      if (res.ok) {
        systemMessage('Image uploaded, OCR text will appear in chat shortly.');
      } else {
        systemMessage('Failed to upload image.');
      }
    } catch (e) {
      systemMessage('Image upload failed.');
    }
  };

  const onSendClick = () => {
    if (!input.trim()) return;
    sendMessage(input.trim());
    setInput('');
  };

  const onRateClick = () => {
    sendRating(rating);
  };

  const onFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;
    processImage(file);
    (e.target as HTMLInputElement).value = '';
  };

  useEffect(() => {
    const connect = async () => {
      const hub = new HubConnectionBuilder().withUrl('/chatHub').build();

      hub.on('ReceiveMessage', (user, message) => {
        addMessage({
          id: crypto.randomUUID(),
          sender: user === 'WebUI' ? 'User' : 'DLP',
          text: message,
          timestamp: formatTimestamp(),
        });
      });

      hub.on('ReceiveRating', (agent, ratingValue) => {
        addMessage({
          id: crypto.randomUUID(),
          sender: agent === 'WebUI' ? 'User' : 'DLP',
          text: `Rating: ${ratingValue}/5`,
          timestamp: formatTimestamp(),
        });
      });

      hub.on('ReceiveImageLog', (log) => {
        addMessage({
          id: crypto.randomUUID(),
          sender: 'InstructionReader',
          text: log,
          timestamp: formatTimestamp(),
        });
      });

      hub.on('ConferenceStarted', () => {
        systemMessage('Conference started. All agents are connected.');
      });

      await hub.start();
      setConnection(hub);
      systemMessage('Connected to backend conference hub.');
      await hub.invoke('StartConference');
    };

    connect().catch((err) => {
      systemMessage('Failed to connect to backend SignalR hub.');
      console.error(err);
    });
  }, []);

  useEffect(() => {
    if (!chatRef.current) return;
    chatRef.current.scrollTop = chatRef.current.scrollHeight;
  }, [messages]);

  const renderedMessages = useMemo(() => {
    return messages.map((m) => (
      <div key={m.id} className={`message ${m.sender === 'User' ? 'user' : m.sender === 'System' ? 'system' : 'agent'}`}>
        <div style={{ fontSize: 12, marginBottom: 4, color: '#334155' }}>
          {m.sender} • {m.timestamp}
        </div>
        <div>{m.text}</div>
      </div>
    ));
  }, [messages]);

  return (
    <div className="container">
      <div className="card">
        <div className="card-header">Deep Learning Protocol Chat</div>
        <div className="card-body">
          <div className="tab-nav">
            <button className={tab === 'chat' ? 'active' : ''} onClick={() => setTab('chat')}>
              Chat
            </button>
            <button className={tab === 'faq' ? 'active' : ''} onClick={() => setTab('faq')}>
              FAQ
            </button>
            <button className={tab === 'privacy' ? 'active' : ''} onClick={() => setTab('privacy')}>
              Privacy
            </button>
          </div>

          {tab === 'chat' && (
            <>
              <div ref={chatRef} className="chat-container">
                {renderedMessages}
              </div>

              <div className="actions">
                <input
                  type="text"
                  value={input}
                  onChange={(e) => setInput(e.target.value)}
                  placeholder="Type a message, ask a question, or say 'faq'..."
                  onKeyDown={(e) => {
                    if (e.key === 'Enter') onSendClick();
                  }}
                />
                <button onClick={onSendClick}>Send</button>
                <select value={rating} onChange={(e) => setRating(e.target.value)}>
                  <option value="1">★</option>
                  <option value="2">★★</option>
                  <option value="3">★★★</option>
                  <option value="4">★★★★</option>
                  <option value="5">★★★★★</option>
                </select>
                <button onClick={onRateClick}>Rate</button>
                <input type="file" accept="image/*" onChange={onFileChange} />
              </div>
            </>
          )}

          {tab === 'faq' && (
            <div>
              {FAQ_ENTRIES.map((entry, idx) => (
                <div key={idx} className="card" style={{ marginBottom: 12 }}>
                  <div className="card-header" style={{ background: '#e2e8f0', color: '#0f172a' }}>
                    {entry.question}
                  </div>
                  <div className="card-body">{entry.answer}</div>
                </div>
              ))}
            </div>
          )}

          {tab === 'privacy' && (
            <div>
              <div className="card" style={{ marginBottom: 12 }}>
                <div className="card-header" style={{ background: '#e2e8f0', color: '#0f172a' }}>
                  Privacy & Data Protection
                </div>
                <div className="card-body">
                  {PRIVACY_TEXT.split('\n').map((line, idx) => (
                    <p key={idx} style={{ margin: '0 0 10px' }}>
                      {line}
                    </p>
                  ))}
                </div>
              </div>
              <div className="card">
                <div className="card-header" style={{ background: '#e2e8f0', color: '#0f172a' }}>
                  How this app uses your data
                </div>
                <div className="card-body">
                  <ul>
                    <li>Chat messages are broadcast to connected agents over SignalR.</li>
                    <li>Uploaded images are processed for OCR and the extracted text is shared in chat.</li>
                    <li>No permanent storage is used unless explicitly configured.</li>
                    <li>Data Loss Prevention (DLP) is included to help flag sensitive content.</li>
                  </ul>
                </div>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
