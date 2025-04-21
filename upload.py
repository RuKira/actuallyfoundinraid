import os
import sys
import requests
import re

version_pattern = r'public const string Version = "(\d\.\d\.\d\.\d)";'
re_version_pattern = re.compile(version_pattern)

secret_token = os.environ.get("GITLAB_SECRET_TOKEN", None)
if not secret_token:
    sys.exit("GITLAB_SECRET_TOKEN environment variable not set")

project_id = os.environ.get("GITLAB_PROJECT_ID", None)
if not project_id:
    sys.exit("GITLAB_PROJECT_ID environment variable not set")

package_name = os.environ.get("UPLOAD_PACKAGE_NAME", None)
if not package_name:
    sys.exit("UPLOAD_PACKAGE_NAME environment variable not set")

def version_line(fp):
    for line in fp:
        if "public const string Version" in line.strip():
            return line

version_file = os.environ.get("VERSION_SOURCE_FILE", None)
if not version_file:
    sys.exit("VERSION_SOURCE_FILE environment variable not set")

def get_version():
    with open(version_file, "r") as plugin:
        vl = version_line(plugin)
        version = re_version_pattern.search(vl).group(1)
        return version.replace('"', '')

package_version = get_version()
file_path = os.path.join("bin", "upload", f"{package_name}-v{package_version}.zip")

file_name = os.path.basename(file_path)

api_root = "https://gitlab.com/api/v4"

headers = {
    'PRIVATE-TOKEN': secret_token,
}

uri = f'{api_root}/projects/{project_id}/packages/generic/{package_name}/{package_version}/{file_name}'

with open(file_path, 'rb') as f:
    r_upload = requests.put(uri, headers=headers, data=f)

    if r_upload.status_code != 201 and r_upload.status_code != 200:
        raise ValueError(f"Upload API responded with invalid status {r_upload.status_code}")  # noqa: E501

    upload = r_upload.json()
    print(upload)
