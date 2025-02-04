export class AppError {
  //constructor(public message: string, public code: number) {}
  constructor(public originalError?: any) {
    console.log("AppError constructor called");
    console.log(originalError);
  }
}