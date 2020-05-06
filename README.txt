SeedController:

Tilføj dummy data til databasen
POST: https://localhost:44323/api/Seed/Users


Usercontroller:

Vis alle brugere:
GET: https://localhost:44323/api/User



Vis specifik bruger:
GET: https://localhost:44323/api/User/BrugerID



Opret ny bruger:
POST: https://localhost:44323/api/User
BODY:
{
	"Name": "BrugerNavn",
	"Age": 99,
	"Email": "Email@Adresse.dk",
	"CircleIDs": ["CircleID1","CircleID2"],
	"BlockedUserIDs": ["BlockedUser1", "BlockedUser2"],
	"FollowedUserIDs": ["FollowedUser1", "FollowedUser2"]
}



Opdater eksisterende bruger:
PUT: https://localhost:44323/api/User/BrugerID
BODY:
{
	"Name": "BrugerNavn",
	"Age": 99,
	"Email": "Email@Adresse.dk",
	"CircleIDs": ["CircleID1","CircleID2"],
	"BlockedUserIDs": ["BlockedUser1", "BlockedUser2"],
	"FollowedUserIDs": ["FollowedUser1", "FollowedUser2"]
}



Slet bruger:
DELETE: https://localhost:44323/api/User/BrugerID

Opret en post på en bruger:
POST: https://localhost:44323/api/User/Post
BODY:
{
	"OwnerId": "BrugerID",
	"Created": "2020-04-20",
	"Content":
	{
		"Text":"ContentText",
		"URL":"VideoURL",
		"Length":5,
		"Title":"VideoTitle"
	}
}



Kommenter på en post:
POST: https://localhost:44323/api/User/Comment
BODY:
{
	"PostId": "PostenSomKommentarenSkalVærePå",
	"Text": "Kommentarens indhold"
}



Vis en brugers væg:
GET: https://localhost:44323/api/User/Wall
BODY:
Ids: ["Brugerens Id", "Seerens Id"]



Vis en brugers feed:
GET: https://localhost:44323/api/User/BrugerID



CircleController:



Vis alle circles:
GET: https://localhost:44323/api/Circle



Vis en specifik circle:
GET: https://localhost:44323/api/Circle/CircleID



Opret en ny circle:
POST: https://localhost:44323/api/Circle
BODY:
{
	"Name":"CircleNavn",
	"MemberIDs":["BrugerID1","BrugerID2","BrugerID3"]
}



Opdater en eksisterende circle:
PUT: https://localhost:44323/api/Circle/CircleID
BODY:
{
	"Name":"CircleNavn",
	"MemberIDs":["BrugerID1","BrugerID2","BrugerID3"]
}



Slet en circle:
DELETE: https://localhost:44323/api/Circle/CircleID



Tilføj brugere til en circle
POST: https://localhost:44323/api/Circle/AddUsers
BODY:
["CircleID","BrugerID1","BrugerID2"]



Tilføj en post til en eller flere circles:
POST: https://localhost:44323/api/Circle/Post
BODY:
{
	"circleIds":["CircleID1","CircleID2","CircleID3"],
	"post":
	{
		"Created":"2020-04-20",
		"content":
		{
			"Text":"Text til posten",
			"Title":"VideoTitle",
			"Length":5,
			"URL":"www.linktilvideo.dk"
		}
	}
}

