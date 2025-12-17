import { Component } from '@angular/core';
import { Employee } from '../../Interfaces/Employee';

@Component({
  selector: 'app-employees',
  imports: [],
  templateUrl: './employees.component.html',
  styleUrl: './employees.component.css'
})
export class EmployeesComponent {
 employees : Employee[]=[
  {id:1,name:"Mousa",},
 ]
employeesTableColumns: string[] = [
  "#",
  "Name",
  "Position",
  "Birthdate",
  "Status",
  "Email",
  "Salary",
  "Department",
  "Manager"
];

}
