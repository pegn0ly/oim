# Generated by Django 3.2.9 on 2021-12-10 10:26

from django.db import migrations, models
import uuid


class Migration(migrations.Migration):

    dependencies = [
        ('oim_connector', '0002_savedfightcondition'),
    ]

    operations = [
        migrations.AlterField(
            model_name='savedfightcondition',
            name='id',
            field=models.UUIDField(auto_created=True, default=uuid.uuid4, primary_key=True, serialize=False),
        ),
    ]
