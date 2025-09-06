import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import { App } from './app/app';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';


bootstrapApplication(App, {
  providers: [
    provideRouter(routes)
  ]
});