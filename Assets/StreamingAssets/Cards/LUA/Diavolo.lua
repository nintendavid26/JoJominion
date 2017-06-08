--Diavolo
function Use()
	Enemy.TakeDamage(6);
end

function OnStartBuyPhase()
	Card.Cost=Card.Cost-User.DmgDealt
	if Card.Cost<0 then Card.Cost=0 end
end

function OnEndBuyPhase()
	Card.Cost=8;
end