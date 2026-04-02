## PUBLIC SUBNETS
resource "aws_subnet" "public" {
  for_each = data.aws_availability_zone.all

  vpc_id = aws_vpc.main.id
  availability_zone = each.key
  cidr_block = cidrsubnet(aws_vpc.main.cidr_block, 4, var.az_number[each.value.name_suffix])
}