search_dir=.
for entry in "$search_dir"/*.yaml
do
  tfk8s -f "$entry" -o "${entry%.yaml}"
done