## VPC CONFIGURATION
resource "aws_vpc" "main" {
    cidr_block = cidrsubnet("10.1.0.0/16", 4, var.region_number[data.aws_region.current.name])
    enable_dns_hostnames = true
    enable_dns_support = true

    tags = merge(local.common_tags, {
        Name = "${local.name_prefix}-vpc"
    })
    lifecycle {
      create_before_destroy = true
    }
}