## LOCAL CONFIGURATION
locals {
    env_short = var.environment
    region_short = replace(var.aws_region, "-", "")

    # https://developer.hashicorp.com/terraform/cloud-docs/workspaces/best-practices?page=operational-excellence&page=operational-excellence-workspaces-projects#name-your-workspace
    name_prefix = "${var.business_unit}-${var.project_name}-${var.layer}-${local.env_short}-${local.region_short}"
    common_tags = merge(
    var.common_tags,
    {
      Project     = var.project_name
      Environment = var.environment
      Region = var.aws_region
      ManagedBy   = "terraform"
      Module = "networking"
    }
  )
}