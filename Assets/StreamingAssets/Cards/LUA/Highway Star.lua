--Highway Star
function Use()
	Enemy.TakeDamage(1);
	if User.HasCard("Highway Star",User.InPlay) then User.Draw(1);
	else Enemy.Money=Enemy.Money-1;
	end
end

