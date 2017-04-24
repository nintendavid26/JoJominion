--Notorious BIG
function Use()
	User.Enemy.TakeDamage(2);
end

function OnDestroy()
	Card c=User.SelectCard(User.Hand);
	c.Destroy();
end