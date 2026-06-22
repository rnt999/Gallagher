import { afterEach, describe, expect, it, vi } from 'vitest';
import { calculate } from './api';

describe('calculate', () => {
  afterEach(() => {
    vi.restoreAllMocks();
  });

  it('falls back gracefully when error response is not JSON', async () => {
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue({
        ok: false,
        json: () => Promise.reject(new SyntaxError('Unexpected token <')),
      }),
    );

    await expect(calculate('either', 0.5, 0.2)).rejects.toThrow('Request failed');
  });

  it('uses backend error field when provided', async () => {
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue({
        ok: false,
        json: () => Promise.resolve({ error: 'Invalid JSON payload' }),
      }),
    );

    await expect(calculate('either', 0.5, 0.2)).rejects.toThrow('Invalid JSON payload');
  });
});
