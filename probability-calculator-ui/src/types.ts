export type Operation = 'combined-with' | 'either';

export interface CalculateResponse {
  result: number;
  formula: string;
}

export interface ApiError {
  title: string;
  status: number;
  errors?: string[];
}
