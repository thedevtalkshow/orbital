| Property | Expected Type | Description | Included |
|----------|---------------|-------------|----------|
| **Properties from Event** ||||
| about | Thing | The subject matter of the content. Inverse property: subjectOf | true |
| actor | PerformingGroup or Person | An actor (individual or a group), e.g. in TV, radio, movie, video games etc., or in an event. Actors can be associated with individual items or with a series, episode, clip. Supersedes actors. | false |
| aggregateRating | AggregateRating | The overall rating, based on a collection of reviews or ratings, of the item. |
| attendee | Organization or Person | A person or organization attending the event. Supersedes attendees. | true |
| audience | Audience | An intended audience, i.e. a group for whom something was created. Supersedes serviceAudience. | true | 
| composer | Organization or Person | The person or organization who wrote a composition, or who is the composer of a work performed at some event. | false | 
| contributor | Organization or Person | A secondary contributor to the CreativeWork or Event. | false |
| director | Person | A director of e.g. TV, radio, movie, video gaming etc. content, or of an event. Directors can be associated with individual items or with a series, episode, clip. Supersedes directors. | false |
| doorTime | DateTime or Time | The time admission will commence. | true |
| duration | Duration or QuantitativeValue | The duration of the item (movie, audio recording, event, etc.) in ISO 8601 duration format. | true |
| endDate | Date or DateTime | The end date and time of the item (in ISO 8601 date format). | true |
| eventAttendanceMode | EventAttendanceModeEnumeration | The eventAttendanceMode of an event indicates whether it occurs online, offline, or a mix. | true | 
| eventSchedule | Schedule | Associates an Event with a Schedule. There are circumstances where it is preferable to share a schedule for a series of repeating events rather than data on the individual events themselves. For example, a website or application might prefer to publish a schedule for a weekly gym class rather than provide data on every event. A schedule could be processed by applications to add forthcoming events to a calendar. An Event that is associated with a Schedule using this property should not have startDate or endDate properties. These are instead defined within the associated Schedule, this avoids any ambiguity for clients using the data. The property might have repeated values to specify different schedules, e.g. for different months or seasons. | false |
| eventStatus | EventStatusType | An eventStatus of an event represents its status; particularly useful when an event is cancelled or rescheduled. | true |
| funder | Organization or Person | A person or organization that supports (sponsors) something through some kind of financial contribution. | false |
| funding | Grant | A Grant that directly or indirectly provide funding or sponsorship for this item. See also ownershipFundingInfo. Inverse property: fundedItem | false |
| inLanguage | Language or Text | The language of the content or performance or used in an action. Please use one of the language codes from the IETF BCP 47 standard. See also availableLanguage. Supersedes language. | true |
| isAccessibleForFree | Boolean | A flag to signal that the item, event, or place is accessible for free. Supersedes free. | true |
| keywords | DefinedTerm or Text or URL | Keywords or tags used to describe some item. Multiple textual entries in a keywords list are typically delimited by commas, or by repeating the property. | true |
| location | Place or PostalAddress or Text or VirtualLocation | The location of, for example, where an event is happening, where an organization is located, or where an action takes place. | true |
| maximumAttendeeCapacity | Integer | The total number of individuals that may attend an event or venue. | true |
| maximumPhysicalAttendeeCapacity | Integer | The maximum physical attendee capacity of an Event whose eventAttendanceMode is OfflineEventAttendanceMode (or the offline aspects, in the case of a MixedEventAttendanceMode). | true |
| maximumVirtualAttendeeCapacity | Integer | The maximum virtual attendee capacity of an Event whose eventAttendanceMode is OnlineEventAttendanceMode (or the online aspects, in the case of a MixedEventAttendanceMode). | false |
| offers | Demand or Offer | An offer to provide this item—for example, an offer to sell a product, rent the DVD of a movie, perform a service, or give away tickets to an event. Use businessFunction to indicate the kind of transaction offered, i.e. sell, lease, etc. This property can also be used to describe a Demand. While this property is listed as expected on a number of common types, it can be used in others. In that case, using a second type, such as Product or a subtype of Product, can clarify the nature of the offer. Inverse property: itemOffered | false |
| organizer | Organization or Person | An organizer of an Event. | true |
| performer | Organization or Person | A performer at the event—for example, a presenter, musician, musical group or actor. Supersedes performers. | true |
| previousStartDate | Date | Used in conjunction with eventStatus for rescheduled or cancelled events. This property contains the previously scheduled start date. For rescheduled events, the startDate property should be used for the newly scheduled start date. In the (rare) case of an event that has been postponed and rescheduled multiple times, this field may be repeated. | true |
| recordedIn | CreativeWork | The CreativeWork that captured all or part of this Event. Inverse property: recordedAt | false |
| remainingAttendeeCapacity | Integer | The number of attendee places for an event that remain unallocated. | true |
| review | Review | A review of the item. Supersedes reviews. | false |
| sponsor | Organization or Person | A person or organization that supports a thing through a pledge, promise, or financial contribution. E.g. a sponsor of a Medical Study or a corporate sponsor of an event. | false | 
| startDate | Date or DateTime | The start date and time of the item (in ISO 8601 date format). | true |
| subEvent | Event | An Event that is part of this event. For example, a conference event includes many presentations, each of which is a subEvent of the conference. Supersedes subEvents. Inverse property: superEvent | true |
| superEvent | Event | An event that this event is a part of. For example, a collection of individual music performances might each have a music festival as their superEvent. Inverse property: subEvent | true |
| translator | Organization or Person | Organization or person who adapts a creative work to different languages, regional differences and technical requirements of a target market, or that translates during some event. | false |
| typicalAgeRange | Text | The typical expected age range, e.g. '7-9', '11-'. | false |
| workFeatured | CreativeWork | A work featured in some event, e.g. exhibited in an ExhibitionEvent. Specific subproperties are available for workPerformed (e.g. a play), or a workPresented (a Movie at a ScreeningEvent). | false | 
| workPerformed | CreativeWork | A work performed in some event, for example a play performed in a TheaterEvent. | false |
| **Properties from Thing** |||
| additionalType | Text or URL | An additional type for the item, typically used for adding more specific types from external vocabularies in microdata syntax. This is a relationship between something and a class that the thing is in. Typically the value is a URI-identified RDF class, and in this case corresponds to the use of rdf:type in RDF. Text values can be used sparingly, for cases where useful information can be added without their being an appropriate schema to reference. In the case of text values, the class label should follow the schema.org style guide. |
| alternateName | Text | An alias for the item. | false |
| description | Text or TextObject | A description of the item. | true |
| disambiguatingDescription | Text | A sub property of description. A short description of the item used to disambiguate from other, similar items. Information from other properties (in particular, name) may be necessary for the description to be useful for disambiguation. | false |
| identifier | PropertyValue or Text or URL | The identifier property represents any kind of identifier for any kind of Thing, such as ISBNs, GTIN codes, UUIDs etc. Schema.org provides dedicated properties for representing many of these, either as textual strings or as URL (URI) links. See background notes for more details. | true |
| image | ImageObject or URL | An image of the item. This can be a URL or a fully described ImageObject. | false |
| mainEntityOfPage | CreativeWork or URL | Indicates a page (or other CreativeWork) for which this thing is the main entity being described. See background notes for details. Inverse property: mainEntity | false |
| name | Text | The name of the item. | true |
| potentialAction | Action | Indicates a potential Action, which describes an idealized action in which this thing would play an 'object' role. | false | 
| sameAs | URL | URL of a reference Web page that unambiguously indicates the item's identity. E.g. the URL of the item's Wikipedia page, Wikidata entry, or official website. | false |
| subjectOf | CreativeWork or Event | A CreativeWork or Event about this Thing. Inverse property: about | false | 
| url | URL | URL of the item. | true |