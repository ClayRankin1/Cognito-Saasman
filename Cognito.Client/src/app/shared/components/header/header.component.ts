import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { UserRoles } from 'src/app/auth/user.roles.enum';
import { ContextMenuService } from '../../services/context.menu.service';

@Component({
    selector: 'app-header',
    templateUrl: 'header.component.html',
    styleUrls: ['./header.component.scss'],
})
export class HeaderComponent {
    @Output() menuToggle = new EventEmitter<boolean>();
    @Input() menuToggleEnabled = false;
    @Input() title: string;
    contactData: {};
    contactPopupVisible = false;
    istasksPressed = true;
    isitemsPressed = true;
    istabsPressed = true;
    tasksthree = true;
    taskstwo = false;
    tasksone = false;
    itemsthree = true;
    itemstwo = false;
    itemsone = false;
    tabsthree = true;
    tabstwo = false;
    tabsone = false;
    userMenuItems: any[];

    constructor(
        private router: Router,
        private auth: AuthService,
        private menuService: ContextMenuService
    ) {
        this.buildMenuItems();
        this.contactData = {
            subject: '',
            message: '',
        };
    }
    toggleMenu = (): void => {
        this.menuToggle.emit();
    };

    onLogoClick = (): void => {
        this.router.navigate(['/']);
    };

    contact = (): void => {
        this.contactPopupVisible = true;
    };

    onContactSubmit = (): void => {
        this.contactPopupVisible = false;
    };

    handleTasks(): void {
        this.istasksPressed = !this.istasksPressed;
        if (this.istasksPressed) {
            if (this.isitemsPressed && !this.istabsPressed) {
                this.taskstwo = true;
                this.tasksthree = false;
                this.tasksone = false;

                this.itemstwo = true;
                this.itemsone = false;
                this.itemsthree = false;
            }
            if (!this.isitemsPressed && this.istabsPressed) {
                this.taskstwo = true;
                this.tasksthree = false;
                this.tasksone = false;

                this.tabstwo = true;
                this.tabsone = false;
                this.tabsthree = false;
            }
            if (this.isitemsPressed && this.istabsPressed) {
                this.tasksthree = true;
                this.taskstwo = false;
                this.tasksone = false;

                this.itemstwo = false;
                this.itemsone = false;
                this.itemsthree = true;

                this.tabstwo = false;
                this.tabsone = false;
                this.tabsthree = true;
            }
            if (!this.isitemsPressed && !this.istabsPressed) {
                this.tasksone = true;
                this.taskstwo = false;
                this.tasksthree = false;
            }
        } else {
            if (this.isitemsPressed && !this.istabsPressed) {
                this.itemsone = true;
                this.itemstwo = false;
                this.itemsthree = false;
            }
            if (!this.isitemsPressed && this.istabsPressed) {
                this.tabsone = true;
                this.tabstwo = false;
                this.tabsthree = false;
            }
            if (this.isitemsPressed && this.istabsPressed) {
                this.itemstwo = true;
                this.itemsone = false;
                this.itemsthree = false;
                this.tabstwo = true;
                this.tabsone = false;
                this.tabsthree = false;
            }
        }
        this.adjustPanel();
    }
    handleItems(): void {
        this.isitemsPressed = !this.isitemsPressed;
        if (this.isitemsPressed) {
            if (this.istasksPressed && !this.istabsPressed) {
                this.itemstwo = true;
                this.itemsthree = false;
                this.itemsone = false;

                this.tasksthree = false;
                this.taskstwo = true;
                this.tasksone = false;
            }
            if (!this.istasksPressed && this.istabsPressed) {
                this.itemstwo = true;
                this.itemsthree = false;
                this.itemsone = false;

                this.tabsone = false;
                this.tabstwo = true;
                this.tabsthree = false;
            }
            if (this.istasksPressed && this.istabsPressed) {
                this.tasksthree = true;
                this.taskstwo = false;
                this.tasksone = false;

                this.itemstwo = false;
                this.itemsone = false;
                this.itemsthree = true;

                this.tabstwo = false;
                this.tabsone = false;
                this.tabsthree = true;
            }
            if (!this.istasksPressed && !this.istabsPressed) {
                this.itemsone = true;
                this.itemstwo = false;
                this.itemsthree = false;
            }
        } else {
            if (this.istasksPressed && !this.istabsPressed) {
                this.tasksone = true;
                this.taskstwo = false;
                this.tasksthree = false;
            }
            if (!this.istasksPressed && this.istabsPressed) {
                this.tabsone = true;
                this.tabstwo = false;
                this.tabsthree = false;
            }
            if (this.istasksPressed && this.istabsPressed) {
                this.taskstwo = true;
                this.tasksone = false;
                this.tasksthree = false;
                this.tabstwo = true;
                this.tabsone = false;
                this.tabsthree = false;
            }
        }
        this.adjustPanel();
    }
    handleTabs(): void {
        this.istabsPressed = !this.istabsPressed;

        if (this.istabsPressed) {
            if (this.istasksPressed && !this.isitemsPressed) {
                this.tabstwo = true;
                this.tabsthree = false;
                this.tabsone = false;

                this.tasksthree = false;
                this.taskstwo = true;
                this.tasksone = false;
            }
            if (!this.istasksPressed && this.isitemsPressed) {
                this.itemstwo = true;
                this.itemsthree = false;
                this.itemsone = false;

                this.tabsone = false;
                this.tabstwo = true;
                this.tabsthree = false;
            }
            if (this.istasksPressed && this.isitemsPressed) {
                this.tasksthree = true;
                this.taskstwo = false;
                this.tasksone = false;

                this.itemstwo = false;
                this.itemsone = false;
                this.itemsthree = true;

                this.tabstwo = false;
                this.tabsone = false;
                this.tabsthree = true;
            }
            if (!this.istasksPressed && !this.isitemsPressed) {
                this.tabstwo = false;
                this.tabsone = true;
                this.tabsthree = false;
            }
        } else {
            if (this.istasksPressed && !this.isitemsPressed) {
                this.tasksone = true;
                this.taskstwo = false;
                this.tasksthree = false;
            }
            if (!this.istasksPressed && this.isitemsPressed) {
                this.itemstwo = false;
                this.itemsone = true;
                this.itemsthree = false;
            }
            if (this.istasksPressed && this.isitemsPressed) {
                this.taskstwo = true;
                this.tasksone = false;
                this.tasksthree = false;

                this.itemstwo = true;
                this.itemsone = false;
                this.itemsthree = false;
            }
        }
        this.adjustPanel();
    }
    adjustPanel(): void {
        document.getElementById('tasksmaindiv').style.display = !this.istasksPressed
            ? 'none'
            : 'block';
        document.getElementById('itemsmaindiv').style.display = !this.isitemsPressed
            ? 'none'
            : 'block';
        document.getElementById('tabsmaindiv').style.display = !this.istabsPressed
            ? 'none'
            : 'block';

        const taskClassName: string = this.tasksone
            ? 'tasksone'
            : this.taskstwo
            ? 'taskstwo'
            : 'tasksthree';
        const itemClassName: string = this.itemsone
            ? 'itemsone'
            : this.itemstwo
            ? 'itemstwo'
            : 'itemsthree';
        const tabClassName: string = this.tabsone
            ? 'tabsone'
            : this.tabstwo
            ? 'tabstwo'
            : 'tabsthree';

        document.getElementById('tasksmaindiv').className = 'box ' + taskClassName;
        document.getElementById('itemsmaindiv').className = 'box ' + itemClassName;
        document.getElementById('tabsmaindiv').className = 'box ' + tabClassName;
    }

    private buildMenuItems(): void {
        this.userMenuItems = [
            this.menuService.createRouteItem('Profile', '/profile'),
            this.createSettingsMenuItem(),
            this.menuService.createRouteItem('Help', '/'),
            this.menuService.createActionItem('Contact Support', () => {
                this.contact();
            }),
            this.menuService.createActionItem('Logout', () => {
                const { refreshToken } = this.auth.currentUser;
                this.auth.logout({ refreshToken });
                this.router.navigate(['/login']);
            }),
        ];
    }

    private createSettingsMenuItem(): any {
        const menuItems = [
            this.menuService.createRouteItem(
                'Tenants',
                '/admin/tenants',
                [UserRoles.SysAdmin],
                [UserRoles.TenantAdmin]
            ),
            this.menuService.createRouteItem('Users', '/admin/users'),
            this.menuService.createRouteItem('Projects', '/admin/projects', null, [
                UserRoles.DomainAdmin,
            ]),
            this.menuService.createRouteItem('Licenses', '/'),
            this.menuService.createParentItem('Types', [
                this.menuService.createRouteItem('Details', '/'),
                this.menuService.createRouteItem('Tasks', '/'),
            ]),
        ];
        return this.menuService.createParentItem('Settings', menuItems);
    }
}
