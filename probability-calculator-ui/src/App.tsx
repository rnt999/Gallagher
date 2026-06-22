import { useState } from 'react';
import { calculate } from './api';
import type { CalculateResponse, Operation } from './types';
import './App.css';

const OPERATIONS: { id: Operation; label: string; formula: string }[] = [
  { id: 'combined-with', label: 'Combined With', formula: 'P(A) × P(B)' },
  { id: 'either',        label: 'Either',        formula: 'P(A) + P(B) − P(A)P(B)' },
];

export default function App() {
  const [operation, setOperation] = useState<Operation>('combined-with');
  const [a, setA] = useState('');
  const [b, setB] = useState('');
  const [result, setResult] = useState<CalculateResponse | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const switchOperation = (op: Operation) => {
    setOperation(op);
    setResult(null);
    setError(null);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setResult(null);
    setLoading(true);
    try {
      setResult(await calculate(operation, Number.parseFloat(a), Number.parseFloat(b)));
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Unexpected error');
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="card">
      <h1>Probability Calculator</h1>

      <div className="tabs" role="tablist">
        {OPERATIONS.map((op) => (
          <button
            key={op.id}
            role="tab"
            aria-selected={operation === op.id}
            className={operation === op.id ? 'tab active' : 'tab'}
            onClick={() => switchOperation(op.id)}
          >
            <span className="tab-label">{op.label}</span>
            <span className="tab-formula">{op.formula}</span>
          </button>
        ))}
      </div>

      <form onSubmit={handleSubmit} className="form">
        <div className="field">
          <label htmlFor="a">Probability A</label>
          <input
            id="a"
            type="number"
            min="0"
            max="1"
            step="0.01"
            placeholder="0.00 – 1.00"
            value={a}
            onChange={(e) => setA(e.target.value)}
            required
          />
        </div>
        <div className="field">
          <label htmlFor="b">Probability B</label>
          <input
            id="b"
            type="number"
            min="0"
            max="1"
            step="0.01"
            placeholder="0.00 – 1.00"
            value={b}
            onChange={(e) => setB(e.target.value)}
            required
          />
        </div>
        <button type="submit" className="btn-primary" disabled={loading}>
          {loading ? 'Calculating…' : 'Calculate'}
        </button>
      </form>

      {error && <div className="banner error" role="alert">{error}</div>}

      {result && (
        <output className="banner result">
          <span className="result-value">{result.result}</span>
          <span className="result-formula">{result.formula}</span>
        </output>
      )}
    </main>
  );
}
