import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AdminService } from './admin.service';

@Component({
    selector: 'app-admin',
    templateUrl: './admin.component.html',
    styleUrls: ['./admin.component.scss'],
})
export class AdminComponent implements OnInit {
    public title$: Observable<string>;

    constructor(private adminService: AdminService) {}

    ngOnInit(): void {
        this.title$ = this.adminService.title$;
    }
}
