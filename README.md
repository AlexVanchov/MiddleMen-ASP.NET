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
 * Validations
 * Authorizations for admin pages
 * Object relations to update
 * Apply for seller
 * Alerts(Bootstrap)
 * Error pages and Alert Redirects
 * Email login and username display after
 * Error view (Bootstrap)
 * Possible to add yt embed to offer(Not sure???)
 * Favorites
 * Many pictures with displayng on crate offer
 * Canvas stats
 * Basket with admin live baskets check
 * Fix idea with comments and reviews (maybe should be separated)
 * Admin on post active functions
 * Offer creator options to edit directly from details page (Edit redirect btn only for him)
 * Messages with live updates
 
 ### Done:
 * onclick item displaying
 * Home page listingitems in category
 * Cloudinary(Cloud)
 * Username on regis
 * Email send (verification/welcome) only
 * Cloud out of limit error
 * Ratings
 * Comments halfly done
 * My Offers active
 * User rating
 * My Offers unactive
 * Serach menu
 * Email send (recover password)
 * Buy button
 * Profile view
 * Profiles
 * Delete/Edit for creator of the offer
 * User profiles with pictures and recent offers
 * Identity pages customise
 
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
