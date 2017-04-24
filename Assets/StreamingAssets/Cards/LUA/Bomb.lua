--Bomb
function OnDraw()
	User.TakeDamage(10);
	--User.Draw(1);
	Card.Destroy(User,User.Hand);
end