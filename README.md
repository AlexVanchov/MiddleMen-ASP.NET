# MiddleMan


[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

MiddleMan is web service for trading accounts, selling game items and buying items
# Features:
  - Accounts with full access (e-mail change and password)
  - Game items (CS:GO, Dota 2 , etc)



### Tech

* [ASP.NET] - MVC
* [node.js] - evented I/O for the backend
* [Cloudinary] - Web cloud for the website

### TODO:

 * Admin edit reordering
    * Make hidden form with name="categoryId" and name="orderNumber" with Submint btn which is on the page where the JavaSctipt will fill with the orderNumber
 * Delete/Edit for creator of the offer
 * Serach menu
 * Comments(with coments for them), ratings, profiles
 * Cloud out of limit error
 * Validations
 * Authorizations for admin pages
 * Profile view
 * Object relations to update
 * Identity pages customise
 * Apply for seller
 * Buy button
 * Alerts(Bootstrap)
 * Error pages and Alert Redirects
 * Email login and username display after
 * Error view (Bootstrap)
 * Email send (verification/welcome/recover password)
 * Possible to add yt embed to offer(Not sure???)
 * Favorites
 * User profiles with pictures and recent offers
 * User rating
 
 ### Done:
 * onclick item displaying
 * Home page listingitems in category
 * Cloudinary(Cloud)
 * Username on regis
 
 ## Objects relations:
* BaseOnDeleteModel addon
    * IsDeleted(bool)
    * DeletedOn(nullable DateTime)
* BaseOnCreateModel addon
    * CreatedOn(DateTime)
    * ModifiedOn(nullable DateTime)
* Categoty : BaseOnDeleteModel, BaseOnCreateModel
    * Id(hash-string)
    * Name(string)
* Offer : BaseOnDeleteModel, BaseOnCreateModel
    * Id(hash-string)
    * CategoryId(foreign key(Category))
    * Name(string)
    * Price(decimal)
    * Description(string)
    * picUrl(nullable string)
* Comment (IMPLEMENTATION NEEDED here + : BaseModel)

License
----

MIT


**Open Source**

   [node.js]: <http://nodejs.org>
   [ASP.NET]: <https://dotnet.microsoft.com/apps/aspnet>
   [Cloudinary]: <https://cloudinary.com/documentation/dotnet_integration>
