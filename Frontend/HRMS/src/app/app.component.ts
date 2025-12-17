import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NgIf, NgFor, NgClass, NgStyle, } from '@angular/common';
import { FormsModule, FormGroup,FormControl,ReactiveFormsModule } from '@angular/forms';
import { RandomColorDirective } from './directives/random-color.directive';
import { EmployeesComponent } from './Components/employees/employees.component';
// Decorator
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NgIf, NgFor, NgClass, NgStyle, RandomColorDirective,FormsModule,
    ReactiveFormsModule,EmployeesComponent ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {

}

