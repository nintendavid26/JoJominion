--Diavolo
function Use()
	Enemy.TakeDamage(6);
end

function OnStartBuyPhase()
	Card.Cost=Card.Cost-User.DmgDealt
end

function OnEndBuyPhase()
	Card.Cost=Card.Cost+User.DmgDealt
end