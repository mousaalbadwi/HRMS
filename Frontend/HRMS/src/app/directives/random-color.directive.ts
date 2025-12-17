import { Directive, ElementRef } from '@angular/core';

@Directive({
  selector: '[appRandomColor]'
})
export class RandomColorDirective {

  constructor(private element : ElementRef) { 
    const colors = ['red', 'blue', 'green', 'purple', 'orange','yellow'];
    let colorNo = Math.floor(Math.random() * colors.length);

    element.nativeElement.style.backgroundColor = colors[colorNo];
  }
}