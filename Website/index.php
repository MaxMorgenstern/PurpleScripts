<html>
<head>
<title>PurpleAccount Validator</title>
</head>
<body>
<?php
$config = include('config.php');

if (isset($_GET["token"])) {	
	list($guid, $email) = explode(":", $_GET["token"]);

	$dbconnection = mysql_connect($config["host"],$config["username"],$config["password"]);
	mysql_select_db($config["database"]) or die("No connection to database!\n");
	
	$query = "SELECT guid FROM `account` WHERE SHA1(email) = '".mysql_real_escape_string($email)."' ".
				"AND SHA1(guid) = '".mysql_real_escape_string($guid)."' ".
				"LIMIT 1";

	$queryResult = mysql_query($query, $dbconnection);
	while ($lineData = mysql_fetch_assoc($queryResult)) {
		$query = "UPDATE `account` SET `active` = '1', `email_verification` = now() WHERE `guid` = '".$lineData["guid"]."' LIMIT 1;";
		mysql_query($query, $dbconnection);
		$affected = mysql_affected_rows();
		if($affected > 0)
			echo "Your account has been validated...";
		exit;
	}

	echo "Can not validate account...";

} else {
	Header("WWW-authenticate: basic realm=\"server\"");
	Header("HTTP/1.0 401 Unauthorized");
	exit;
}
?>
</body>
</html>
