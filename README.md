# DieteticConsultationAPI

## Table of Contents
* [General Info](#general-information)
* [Technologies Used](#technologies-used)
* [Features](#features)

## General Information
DieteticConsultationAPI is an application developed for dieticians and their patients for better cooperation and communication, also for support during the diet. This application created for educational purposes. I chose this kind of application because I would like to make the work of my wife who is a dietician, so that she will be happier and have more time for me.

## Technologies Used
- ASP.NET Core 6.0
- Entity Framework Core 6.0.9
- SQL Server 6.0.9
- JwtBearer 6.0.9
- XUnit 2.4.1
- FluentAssertions 6.7.0 
- Moq 4.18.2

## Features
- registration and logging in via the generated token,
- authorization based on the rights granted by the administrator.

### Features for a dietitian:
- creating a profile, providing the most important contact details and a short biography, in which he has the opportunity to specify his specialization and scope of services. Thanks to this, patients will be able to choose the dietitian best suited to their needs.
- searching for a patient in his dietician's database,
- viewing the list of your patients,
- viewing, uploading and downloading files on the patient's account,
- editing your profile, information about yourself, changing your specialization, etc.
- editing changes in dimensions or weight, as well as uploading files to the patient's profile, incl. individual recommendations or menus.
- adding and removing a patient

### Features for the patient:
- creating a profile, providing the most important information about yourself and your dietary requirements, such as your favorite or prohibited products. This information will be added to the patient's database in his part of the system, thanks to which the visiting dietitian will learn about his ailments and needs faster, and during the visit, the specialist will be able to familiarize him with a new, more effective nutritional program.
- editing changes in weight and dimensions, which can be observed both by himself and the specialist taking care of him.
- uploading and downloading files attached by a dietitian, i.e. an individual menu or recommendations.




