var http = require('http');
var mysql = require('mysql2');
var express = require('express');
var cors = require('cors');
var bodyParser = require('body-parser');


const hostname = '127.0.0.1';
const port = 3000;

dbcon = mysql.createConnection({
	host: 'localhost',
	user: 'root',
	password: 'Macaco01',
	database: 'SleepingElements'
});

dbcon.connect(function(err) {
	if (err) throw err;
	console.log("MySQL Connected!");
});

var app = express();
app.use(cors())
app.use(bodyParser.json());


app.listen(port, hostname, () => console.log(`Server running at 
			http://${hostname}:${port}/`));


//---------------Login------------------------
app.post('/players/login', (req, res, next) => {
	console.log(req.body);
	var data = req.body;

	var username = data.Username;
	var password = data.Password;
	


	dbcon.query('SELECT * FROM User WHERE Username=?', [username], function (err, result, fields) {
		//console.log("now here");
		dbcon.on('error', function (err) {
			console.log('[MYSQL ERROR]', err);
		})

		//console.log(result);

		if (result && result.length) {
			if (password == result[0].Password) {
				result[0].Nono = 0;
				res.end(JSON.stringify(result[0]));
			}
			else {//problem 1 wrong pass
				res.json({ "Nono": 1 });
			}
		}
		else {//problem 2 no user with that name   
			res.json({ "Nono": 2 });
		}
	})
});


//-----------------Register-------------------------
app.post('/players/register', (req, res, next) => {
	//console.log(req.body);
	var data = req.body;

	var username = data.username;
	var password = data.password;
	

	dbcon.query('SELECT Username FROM User WHERE Username=?', [username], function (err, result, fields) {
		console.log("now here");
		dbcon.on('error', function (err) {
			console.log('[MYSQL ERROR]', err);
		})

		console.log(result);

		if (result && result.length) {//problem 1 user with that name 

			res.json({ "Problem": 1 });

		}
		else {  

			/*if (password == result[0].Password) {
				result[0].Nono = 0;
				res.end(JSON.stringify(result[0]));
			}
			else {//problem 1 wrong pass
				res.json({ "Nono": 1 });
			}*/

			/*let values = [
			username,
			password
			];*/

			dbcon.query('INSERT INTO User(Username, Password) VALUES (?,?)', [Username, Password], function (err, result, fields) {



				//console.log("now here");
				dbcon.on('error', function (err) {
				console.log('[MYSQL ERROR]', err);

				})

				res.json({ "Problem": 0 });
			})

		}
	})
});


//---------------Pet------------------------
app.post('/players/pet', (req, res, next) => {
	console.log(req.body);
	var data = req.body;

	var petHp = data.PetHealthBar;
	var userID = data.User_ID;
	

	dbcon.query('SELECT PetHealthBar, User_ID FROM Pets WHERE User_ID=?', [userID], function (err, result, fields) {
		console.log("now here");
		dbcon.on('error', function (err) {
			console.log('[MYSQL ERROR]', err);
		})

		console.log(result);

		if (result && result.length) {

				result[0].Problem = 0;
				res.end(JSON.stringify(result[0]));

		}

	})
});
