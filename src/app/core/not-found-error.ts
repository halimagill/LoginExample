import { AppError } from "./AppError";

export class NotFoundError extends AppError {
  constructor(originalError?: any) {
    super(originalError);
  }
}