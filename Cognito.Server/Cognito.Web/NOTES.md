# when deployment succeeds but app login inconsitetnly fails:
If you go to the AWS console, the load balancer has a monitor that you can check here: 
https://console.aws.amazon.com/ec2/v2/home?region=us-east-1#TargetGroups:sort=targetGroupName
A deployment fails if it is unable to build the source code or deploy to their elastic container service. 
Both of those succeeded. The problem has to do with the load balancer that tells the container service 
when to kill a process and spin up a new one. Since the health check was failing, 
it was killing the process and restarting it over and over which is why you could log in randomly. 
The build was fine, but AWS couldn't verify that using the health check endpoint
I've added a healthcheck now:
http://memento-elb-1241727451.us-east-1.elb.amazonaws.com/health

#   when deploment fails
here are logs that you can view for the running tasks: https://console.aws.amazon.com/ecs/home?region=us-east-1#/clusters/memento-cluster/tasks
new messages you can click on each running task and view the logs under the log tab 
just click on the task id (it looks like a GUID usually)

#TO-DO
calendar click and drop:
https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxDateBox/Configuration/#openOnFieldClick

PDFTron "ctrl-c":



 Other issues:
 Clear local storage of login creds - login has to be refreshed to work currently

 #FIX BUG:
 https://www.devexpress.com/Support/Center/Question/Details/T849449/datagrid-form-the-cannot-read-property-resetoption-of-undefined-error-occurs-in-an

# PUSH API !!! open API folder in powershell FIRST
aws ecr get-login --no-include-email --region us-east-1

Invoke-Expression -Command (aws ecr get-login --no-include-email --region us-east-1)

docker build -t memento/api .

docker tag memento/api:latest 345448615840.dkr.ecr.us-east-1.amazonaws.com/memento/api:latest

docker push 345448615840.dkr.ecr.us-east-1.amazonaws.com/memento/api:latest

Login to the Saasman AWS account.
Navigate to the AWS CodeDeploy service.
Click application on the left-hand pane.
Select the first application titled AppECS-memento-cluster-memento-ecs-service.
Select the Deployments tab and then click on the Create Deployment button.
For Deployment group, type DgpECS-memento-cluster-memento-ecs-service.
For revision type, Use AppSpec editor, then select YAML as the AppSpec language.
Paste in the following:

version: 0.0
Resources:
  - TargetService:
      Type: AWS::ECS::Service
      Properties:
        TaskDefinition: "arn:aws:ecs:us-east-1:345448615840:task-definition/memento-task-def:1"
        LoadBalancerInfo:
          ContainerName: "memento-api"
          ContainerPort: 5000
        PlatformVersion: "LATEST"

Client:
ng build --prod
aws configure
aws s3 sync dist/ s3://memento.app

#PDFTron - fix/workaround
you can set it here https://s3.console.aws.amazon.com/s3/buckets/memento.app/wv-resources/lib/core/external/?region=us-east-1&tab=overview
select the file and under actions > change metadata > set content-type to 
application/javascript
decode.min.js

#BIG IDEAS
#Avatars auto generated
https://www.npmjs.com/package/ngx-avatar

#SEND AND RECEIVE EMAILS ON .NET CORE
https://dotnetcoretutorials.com/2017/11/02/using-mailkit-send-receive-email-asp-net-core/
or
send emails, html and attachments (files, streams and strings) from node.js to any smtp server
https://www.npmjs.com/package/emailjs

#Drag and Drop
#for My Websites tab?
#for Quick Docs?
https://dev.opera.com/articles/drag-and-drop/demo.html

####create image (canvas) from dropped link URL?
https://github.com/niklasvh/html2canvas --puts image on canvas
**SAVE dropped ;ink to new column/field in new table MySources - column: MyWebsites
https://ctrlq.org/code/19136-screenshots-javascript --creates an image from page url
####OR create iFrame 
for each and resize page to zoom down size - put click event on iFrame element to open
page in tab/browser for item extraction


#Amazing Groupable DnD
http://marceljuenemann.github.io/angular-drag-and-drop-lists/demo/#/nested
Design you own work-space???

#Javascript based text Editor
https://killercodemonkey.github.io/ngx-quill-example/



#INSTANT ELECTRON APP!!
https://github.com/jiahaog/nativefier

#Hosting MSSQL on AWS
https://www.youtube.com/watch?v=DhAl4rdrzoA
https://www.youtube.com/watch?v=aj76RPamXeE


#Turn Angular app into Electron
https://developer.okta.com/blog/2019/03/20/build-desktop-app-with-angular-electron

#Left/right vertical pop out menu - USE FOR CHAT?
https://js.devexpress.com/Demos/WidgetsGallery/Demo/Drawer/LeftOrRightPosition/Angular/Light/


Explore PWA (and electron?) to run on smart device and desktop respectively...

#QuikDocs
You want to add a distinctive document quickly:
Click on QuikDocs
pop up: capture title - color of tab-  and show editor
on save - creates a
Adds a tab with a header

#QuikWeb
You want a web site added with a large clickable-div showing main content (image) of the website
and 

#Desktop "manager"
an electron app that interacts with Memento - but minimizes as a side-bar and is 
(optionally) "always on top"
Has timer?
Has QuikDocs links
Has QuikSites links
etc.


![Image of Yaktocat](https://octodex.github.com/images/yaktocat.png)