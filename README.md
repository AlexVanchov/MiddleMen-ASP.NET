# MiddleMan


[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

# Site - alexvanchov.uk

![MiddleMan](https://i.imgur.com/0YnGKgr.png)
### Test Users:
 * Admin - admin@gmail.com:123456
 * User - user@gmail.com:123456

MiddleMan is web app for selling accounts, game items, codes
# Features:
## Users
  - Accounts with full access (e-mail and password change)
  - Emails for verifications and more
  - Offer create, editing, removing and promoting at home page
  - Uploading images to Clouldinary
  - Comments with rate for offer (to rate you need to add comment, then you can change your rate for offer)
  - Live Messages for offer (using SignalR and vanilla js to append them live)
  - Notification number on icon if any news
  - Search by offer title and description
  - Live chat to staff and admins using hubspot (3rd party)
  - Favorite any offer
  - Buy offer sends email to buyer with account/code and adds info to order history view (leave card info empty to buy)
  - User profile picture, username, emails, firstname, lastname and phone number
  - View user profile by clicking user in offer details redirects to user profile with user info and active offers
  - Category displaying offers with next, previous page
## Admins
  - View all active, declined offers
  - Approve, decline offers after someone create one
  - Last week created offers statistic
  - Menage users (Ban, unban - when someone is banned all offers are deleted and when unbanned they will be visible again)
  - Create, remove category (When category is removed all offer are also deleted)
  - Reorder Categories positions (Using vanilla js and jquery for ajax. To reorder them just drag one where you want)


# Tech:

* [ASP.NET] - MVC
* [node.js] - evented I/O for the backend
* [Cloudinary] - Web cloud for the website
* [SendGrid] - Email Sender API
* [hubspot] - Live Chat Support
* [jquery]  - Ajax requests to the server
* [charts.js] - Graphs for admins
* [SignalR] - Realtime updates (chats)

License
----

MIT


**Open Source**

   [node.js]: <http://nodejs.org>
   [ASP.NET]: <https://dotnet.microsoft.com/apps/aspnet>
   [Cloudinary]: <https://cloudinary.com/documentation/dotnet_integration>
   [Hubspot]: <https://app.hubspot.com/>
   [SendGrid]: <https://sendgrid.com/>
   [jquery]: <https://jquery.com>
   [charts.js]: <https://www.chartjs.org>
   [SignalR]: <https://dotnet.microsoft.com/apps/aspnet/signalr>
