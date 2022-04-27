<br />
  <h1 align="center">Helios clean architecture API template</h1>
  <h3 align="center">Get started on your project fast and keep an organized architecture</h3>

<p align="center">A ready to install  WEB API template built with .Net 5, secured with JWT bearer, and following the principles of clean architecture.</p>

## ✨ Features
<ul>
  <li>MediatR Pattern</li>
  <li>Clean Architecture </li>
  <li>Multiple Database</li>
  <li>
    Authentication flow
    <ul>
      <li>Registration</li>
      <li>Email confirmation</li>
      <li>Authentication</li>
      <li>Refresh Token</li>
      <li>Password resetting</li>
    </ul>
  </li>
  <li>CRUD Operation</li>
  <li>Integration tests</li>
  <li>Unit tests</li>  
</ul>

## :rocket: Quick start

### Installation
 <i>General requirement</i> - <br/>
  In order for those projects to work, you'll need to have: <br/>
  <ul>
  <li>
    An IDE of your choice, but I'd really recommand Visual Studio 2019. Community version would be 100% fine. If you do not own it yet, take a look
    <a href="https://visualstudio.microsoft.com/fr/"> here </a>.
  </li>
    <li>
      An instance of SQL server installed. If you need help with that, here's a <a href="https://computingforgeeks.com/install-sql-server-developer-edition-on-windows-server/">cool guide</a>. 
    </li>
    <li>
    The .Net 5 (<a href="https://dotnet.microsoft.com/download/dotnet/5.0">Click here!</a> )framework installed. 
    </li>
  </ul>

### Step 1: Clone the repo
Clone the repo locally by doing -

```sh
https://github.com/heliosCreation/Helios-API-Template.git
```

### Step 2: Create template locally
Navigate to the root folder of the project containing the .template.config folder. <br/>
Open the command line prompt in the current location and type: 
```sh
dotnet new --install .
```
This should create the local template ready to be used as shown in the caption bellow:  
<kbd>
  <img width="947" alt="DotnetTemplates" src="https://user-images.githubusercontent.com/71494857/165502996-3cea7c22-232b-4ad7-90cf-71cafc683762.PNG">
</kbd>

### Step 3: Use the template
Navigate to the location you want to instantiate the project to, open the command line prompt: and type:
```sh
dotnet new HCA_API -o "ProjectName"
```
You will be prompted with a message stating that the operation went well: 
<kbd>
  <img width="386" alt="validation" src="https://user-images.githubusercontent.com/71494857/165504188-25740a73-c9ed-49d1-b57c-7ccb6da9fc02.PNG">
</kbd>
