# CacheCow Demo

Demonstrates using the [CacheCow ASP.NET Web API Caching Library](https://github.com/aliostad/CacheCow/) within a heirarchical REST API.

Specifically it ensures that caching works for related resources such that:

`POST /contacts` invalidates `/contacts`  
`PUT /contacts/10` invalidates `/contacts/10, /contacts`  
`POST /contacts/10/numbers` invalidates `/contacts/10/numbers, /contacts/10, /contacts`

Although I'm not using it in the example the `LinkedRouteProvider` can be tweaked to set a maximum cache invalidation depth. 

For example if you have quite a deep heirarchy you may want a POST to `/sites/10/contacts/10/numbers` to only invalidate up to `/sites/10/contacts` (not `/sites/10` or above). 
This can be achieved by setting a max invalidation depth:

Change:

	return GetRelatedResources(path);

To:

	// don't invalidate anything above the 3rd resource in the heirarchy
	return GetRelatedResources(path, 3);

## License

Licensed under the MIT License.