# Generated by Django 3.2.9 on 2021-12-10 10:21

from django.db import migrations, models


class Migration(migrations.Migration):

    initial = True

    dependencies = [
    ]

    operations = [
        migrations.CreateModel(
            name='FieldProps',
            fields=[
                ('id', models.IntegerField(primary_key=True, serialize=False)),
                ('BaseX', models.IntegerField()),
                ('BaseY', models.IntegerField()),
                ('Width', models.IntegerField()),
                ('Height', models.IntegerField()),
            ],
        ),
        migrations.CreateModel(
            name='FightProps',
            fields=[
                ('Id', models.BigIntegerField(primary_key=True, serialize=False)),
                ('Stage', models.CharField(choices=[('UNDEFINED', 'UNDEFINED'), ('PREPARE', 'PREPARE'), ('IN_PROGRESS', 'IN_PROGRESS'), ('PAUSED', 'PAUSED'), ('COMPLETED', 'COMPLETED')], max_length=12)),
            ],
        ),
        migrations.CreateModel(
            name='ObstacleMapTemplate',
            fields=[
                ('id', models.IntegerField(auto_created=True, default=0, primary_key=True, serialize=False)),
                ('field_id', models.IntegerField()),
                ('obstacle_map', models.JSONField()),
            ],
        ),
        migrations.CreateModel(
            name='Test',
            fields=[
                ('id', models.BigAutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('asd', models.TextField()),
            ],
        ),
    ]