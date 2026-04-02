## https://stackoverflow.com/questions/63991120/automatically-create-a-subnet-for-each-aws-availability-zone-in-terraform
data "aws_availability_zones" "available" {
    state = "available"
}

data "aws_availability_zone" "all" {
  for_each = toset(data.aws_availability_zones.available.names)

  name = each.key
}

data "aws_region" "current" {}