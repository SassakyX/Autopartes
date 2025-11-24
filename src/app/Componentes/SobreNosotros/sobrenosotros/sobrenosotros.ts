import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';

@Component({
  selector: 'app-sobrenosotros',
  imports: [CommonModule],
  templateUrl: './sobrenosotros.html',
  styleUrl: './sobrenosotros.css',
  standalone: true,
})
export class Sobrenosotros implements OnInit, OnDestroy {
videos: string[] = [
    'https://videocdn.cdnpk.net/videos/fa2d0b26-2801-4ca8-b29f-bce0dacc5d97/horizontal/previews/clear/large.mp4?token=exp=1763073319~hmac=40a8786fd51b112c029e8cf630f0f87424778fc9080c295b93a25ec6f39a7033',
    'https://videocdn.cdnpk.net/videos/463f0267-a1df-48b8-8efd-2f1f1e0dc5b0/horizontal/previews/clear/large.mp4?token=exp=1763073954~hmac=518b13567fecdbc5f5f16096a11d9b87c8983ca094b1b465e9666c12227adde1',
    'https://videocdn.cdnpk.net/videos/0618dfe8-747f-4b90-9d17-5dd736e82212/horizontal/previews/clear/large.mp4?token=exp=1763074897~hmac=e5d9e07b802cc4fd6adbff0f6ee88bc9bc5de0aa2b3f9a1cc408586dc5d5a61d'
  ];

  videoActual: number = 0;
  intervaloRotacion: any;

  ngOnInit(): void {
    this.iniciarRotacionVideos();
  }

  iniciarRotacionVideos(): void {
    this.intervaloRotacion = setInterval(() => {
      this.videoActual = (this.videoActual + 1) % this.videos.length;
    }, 9500);
  }

  ngOnDestroy(): void {
    clearInterval(this.intervaloRotacion);
  }
}


