from django.contrib import admin

# Register your models here.
from .fight.models import FightProps, SavedFightCondition
from .field.models import FieldProps

admin.site.register(FightProps)
admin.site.register(FieldProps)
admin.site.register(SavedFightCondition)