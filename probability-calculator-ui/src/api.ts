import type { CalculateResponse, Operation } from './types';

const BASE = import.meta.env.VITE_API_URL ?? 'http://localhost:7179/api';

export async function calculate(
  operation: Operation,
  probabilityA: number,
  probabilityB: number,
): Promise<CalculateResponse> {
  const res = await fetch(`${BASE}/v1/${operation}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ probabilityA, probabilityB }),
  });

  if (!res.ok) {
    const err = await res.json();
    const messages: string[] = err.errors ?? [err.error ?? err.title ?? 'Request failed'];
    throw new Error(messages.join(' '));
  }

  return res.json() as Promise<CalculateResponse>;
}
