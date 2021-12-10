import uuid
from django.db import models
from django.db.models.fields import BigAutoField, BigIntegerField, CharField, IntegerField, TextField, UUIDField
from django.db.models.lookups import In
#from django.db.models.fields.json import JSONField

class FightProps(models.Model):

    class FightStage(models.TextChoices):
        UNDEFINED = 'UNDEFINED', ('UNDEFINED')
        PREPARE = 'PREPARE', ('PREPARE')
        IN_PROGRESS = 'IN_PROGRESS', ("IN_PROGRESS")
        PAUSED = 'PAUSED', ("PAUSED")
        COMPLETED = 'COMPLETED', ("COMPLETED")

    Id = BigIntegerField(primary_key=True)
    Stage = CharField(choices=FightStage.choices, max_length=12)
    
class SavedFightCondition(models.Model):
    id = UUIDField(primary_key=True, auto_created=True, default=uuid.uuid4)

    FightID = BigIntegerField(default=0)
    Turn = IntegerField(default=0)
    UnitsPositions = TextField(default="")