export const navigation = [
    {
        text: 'Home',
        path: '/home',
        icon: 'home',
    },
    {
        text: 'Admin',
        icon: 'user',
        items: [
            {
                text: 'Domains',
                path: '/domains',
            },
            {
                text: 'Links',
                path: '/links',
            },
            {
                text: 'Projectusers',
                path: '/projectusers',
            },
            {
                text: 'Contacts',
                path: '/contactsadmin',
            },
            {
                text: 'Projects',
                path: '/projects',
            },
            {
                text: 'Users',
                path: '/users',
            },
            {
                text: 'Invite New Users',
                path: '/newusers',
            },
            {
                text: 'Types',
                items: [
                    {
                        text: 'Item Types',
                        path: '/itemtypes',
                    },
                    {
                        text: 'Document Types',
                        path: '/doctypes',
                    },
                    {
                        text: 'Activity Types',
                        path: '/acttypes',
                    },
                ],
            },
        ],
    },
];
