import { describe, it, expect, beforeEach, vi } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import App from './App';
import * as api from './api';

vi.mock('./api');

describe('App', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders the calculator with both operations', () => {
    render(<App />);
    expect(screen.getByRole('heading', { name: 'Probability Calculator' })).toBeInTheDocument();
    expect(screen.getByRole('tab', { name: /Combined With/ })).toBeInTheDocument();
    expect(screen.getByRole('tab', { name: /Either/ })).toBeInTheDocument();
  });

  it('switches between operations', async () => {
    render(<App />);
    const eitherTab = screen.getByRole('tab', { name: /Either/ });
    await userEvent.click(eitherTab);
    expect(eitherTab).toHaveAttribute('aria-selected', 'true');
  });

  it('clears result when switching operations', async () => {
    vi.mocked(api.calculate).mockResolvedValue({
      result: 0.3,
      formula: 'P(A) x P(B)'
    });
    
    render(<App />);
    await userEvent.type(screen.getByLabelText('Probability A'), '0.5');
    await userEvent.type(screen.getByLabelText('Probability B'), '0.6');
    await userEvent.click(screen.getByRole('button', { name: 'Calculate' }));
    
    await waitFor(() => {
      expect(screen.getByText('0.3')).toBeInTheDocument();
    });

    await userEvent.click(screen.getByRole('tab', { name: /Either/ }));
    expect(screen.queryByText('0.3')).not.toBeInTheDocument();
  });

  it('calculates combined-with result', async () => {
    vi.mocked(api.calculate).mockResolvedValue({
      result: 0.3,
      formula: 'P(A) x P(B)'
    });

    render(<App />);
    await userEvent.type(screen.getByLabelText('Probability A'), '0.5');
    await userEvent.type(screen.getByLabelText('Probability B'), '0.6');
    await userEvent.click(screen.getByRole('button', { name: 'Calculate' }));

    await waitFor(() => {
      expect(screen.getByText('0.3')).toBeInTheDocument();
    });
  });

  it('displays error when calculation fails', async () => {
    vi.mocked(api.calculate).mockRejectedValue(new Error('API Error'));

    render(<App />);
    await userEvent.type(screen.getByLabelText('Probability A'), '0.5');
    await userEvent.type(screen.getByLabelText('Probability B'), '0.6');
    await userEvent.click(screen.getByRole('button', { name: 'Calculate' }));

    await waitFor(() => {
      expect(screen.getByRole('alert')).toHaveTextContent('API Error');
    });
  });

  it('shows loading state during calculation', async () => {
    let resolveCalculate: (value: any) => void;
    const calculatePromise = new Promise((resolve) => {
      resolveCalculate = resolve;
    });
    vi.mocked(api.calculate).mockReturnValue(calculatePromise as any);

    render(<App />);
    await userEvent.type(screen.getByLabelText('Probability A'), '0.5');
    await userEvent.type(screen.getByLabelText('Probability B'), '0.6');
    
    const submitBtn = screen.getByRole('button', { name: 'Calculate' });
    await userEvent.click(submitBtn);

    expect(submitBtn).toHaveTextContent(/Calculating/);
    expect(submitBtn).toBeDisabled();

    resolveCalculate!({ result: 0.3, formula: 'P(A) x P(B)' });
    await waitFor(() => {
      expect(submitBtn).toHaveTextContent('Calculate');
      expect(submitBtn).not.toBeDisabled();
    });
  });

  it('clears error when submitting new calculation', async () => {
    render(<App />);
    vi.mocked(api.calculate).mockRejectedValueOnce(new Error('Error 1'));

    await userEvent.type(screen.getByLabelText('Probability A'), '0.5');
    await userEvent.type(screen.getByLabelText('Probability B'), '0.6');
    await userEvent.click(screen.getByRole('button', { name: 'Calculate' }));

    await waitFor(() => {
      expect(screen.getByRole('alert')).toBeInTheDocument();
    });

    vi.mocked(api.calculate).mockResolvedValueOnce({
      result: 0.3,
      formula: 'P(A) x P(B)'
    });

    await userEvent.click(screen.getByRole('button', { name: 'Calculate' }));
    await waitFor(() => {
      expect(screen.queryByRole('alert')).not.toBeInTheDocument();
    });
  });
});
