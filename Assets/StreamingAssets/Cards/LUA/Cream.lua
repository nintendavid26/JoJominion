--Cream
function Use()
	Enemy.TakeDamage(3);
	Card C=User.SelectCard();
	C.Destroy(C,User.Discarded);
end