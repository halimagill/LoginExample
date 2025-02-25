import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
//import { AuthenticationService } from './core/services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  imports: [RouterOutlet],
  styleUrl: './app.component.less'
})
export class AppComponent {
  title = 'LoginExample';
 // constructor(public authService: AuthenticationService) {}
}
