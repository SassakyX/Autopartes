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
    'https://videocdn.cdnpk.net/videos/5d3dabe3-ba07-4892-b407-c04c212d1c3a/horizontal/previews/clear/large.mp4?token=exp=1763988692~hmac=fba5c3464fa76b22d68aa8b41544f92b30664118234d704127a1d69613baddf9',
    'https://videocdn.cdnpk.net/videos/463f0267-a1df-48b8-8efd-2f1f1e0dc5b0/horizontal/previews/clear/large.mp4?token=exp=1763988869~hmac=bbe7a6a580bb345b19951de7c869f92ade49c82df3402b09e48886eedec7a044',
    'https://videocdn.cdnpk.net/videos/25d6fa27-b698-488d-9396-33ad191fd2f3/horizontal/previews/clear/large.mp4?token=exp=1763988438~hmac=aca811c701d6c1cb23fa8258b7486f4bdf34afe4817b3e7c430f549878b10d0c'
  ];

  videoActual: number = 0;
  intervaloRotacion: any;

  ngOnInit(): void {
    this.iniciarRotacionVideos();
  }

  iniciarRotacionVideos(): void {
    this.intervaloRotacion = setInterval(() => {
      this.videoActual = (this.videoActual + 1) % this.videos.length;
    }, 9000);
  }

  ngOnDestroy(): void {
    clearInterval(this.intervaloRotacion);
  }
}


