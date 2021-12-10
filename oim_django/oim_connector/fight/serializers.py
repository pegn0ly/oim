from django.db.models import fields
from rest_framework import serializers
from .models import FightProps, SavedFightCondition

#
class FightInfoSerializer(serializers.ModelSerializer):
    class Meta:
        model = FightProps
        fields = ['Id', 'Stage']
#
class SaveNewFightStateSerializer(serializers.ModelSerializer):
    class Meta:
        model = SavedFightCondition
        fields = ['FightID', 'Turn']
#
class UpdateFightStateSerializer(serializers.ModelSerializer):
    class Meta:
        model = SavedFightCondition
        fields = ['FightID', 'Turn', 'UnitsPositions']