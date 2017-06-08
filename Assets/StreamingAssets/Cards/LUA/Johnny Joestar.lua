--Johnny Joestar
function Use()
	Enemy.TakeDamage(User.Deck[0].Cost);
	User.GetMoney(User.Deck[0].Cost);
	User.Draw(1);
end