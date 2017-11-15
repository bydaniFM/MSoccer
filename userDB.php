<?php 

	/*	-- TO GET TO THE DATABASE VIA THE CMD --

	1.- cd C:\wamp64\bin\mysql\mysql5.7.14\bin

	2.- mysql -u root

	3.- use userDB;

	*/
	$host = "localhost";
	$user = "root";
	$password = "";
	$database = "userDB";

	$mysqli = new mysqli($host, $user, $password, $database);
	//if($mysqli->connect_errno){
	//	echo "error<br>";
	//} else {
	//	echo "success<br>";
	//}

	$username = $_GET["login"];
	$player = $username;
	$password = $_GET["password"];

	$stmt = $mysqli->prepare("SELECT * FROM logindata WHERE username = ? AND password = ?");
	$stmt->bind_param("ss", $username, $password);
	$stmt->execute();

	$result = $stmt->get_result();

	//while($row = $result->fetch_assoc()){
	//	echo $row["username"]." ".$row["password"]."<br>";
	//}

	$row = $result->fetch_assoc();
	if ($row["username"] == $username){
		echo $player;
	} else {
		echo "wrong username/password";
	}

 ?>