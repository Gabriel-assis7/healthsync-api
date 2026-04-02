## NETWORKING VARIABLES
variable "vpc_cidr" {
    type = string
}

variable "environment" {
    type = string
    description = "Environment name"
}

variable "project_name" {
    type = string
    description = "Name of the project"
}

variable "aws_region" {
    type = string
    description = "Aws region"
}

variable "business_unit" {
  type        = string
  default     = null
  description = "Optional: business unit or team"
}

variable "layer" {
  type        = string
  default     = "network"
  description = "Infrastructure layer (network, compute, data, etc.)"
}

variable "region_number" {
  default = {
    us-east-1      = 1
    us-west-1      = 2
    us-west-2      = 3
    eu-central-1   = 4
    ap-northeast-1 = 5
  }
}

variable "az_number" {
  default = {
    a = 1
    b = 2
    c = 3
    d = 4
    e = 5
    f = 6
  }
}

variable "common_tags" {
    type = map(string)
    description = "Common tags to apply to all resources"
    default = {}
}

variable "extra_tags" {
  type        = map(string)
  default     = {}
  description = "Additional tags to merge"
}