import { ErrorHandler, inject, Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class AppErrorHandlerService implements ErrorHandler {
  handleError(error: any): void {
    console.log("Unexpected error occurred: " + error);
    console.log(error);
    console.log('handle and log the error here and send it to the server');
    // console.error('An error occurred:', error);
    //throw error (Keep this line uncommented in development  in order to see the error in the console)
  }
}