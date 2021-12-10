from django.db import models
from django.db.models.fields import IntegerField
from django.db.models.fields.json import JSONField

class FieldProps(models.Model):

    id = IntegerField(primary_key=True)
    BaseX = IntegerField()
    BaseY = IntegerField()
    Width = IntegerField()
    Height = IntegerField()

class ObstacleMapTemplate(models.Model):
    id = IntegerField(default=0, primary_key=True, auto_created=True)
    field_id = IntegerField()
    obstacle_map = JSONField()