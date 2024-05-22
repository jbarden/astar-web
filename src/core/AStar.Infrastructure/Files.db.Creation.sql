BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "TagsToIgnore" (
	"Value"	TEXT NOT NULL COLLATE NOCASE,
	CONSTRAINT "PK_TagsToIgnore" PRIMARY KEY("Value")
);
CREATE TABLE IF NOT EXISTS "TagsToIgnoreCompletely" (
	"Value"	TEXT NOT NULL COLLATE NOCASE,
	CONSTRAINT "PK_TagsToIgnoreCompletely" PRIMARY KEY("Value")
);
CREATE TABLE IF NOT EXISTS "Files" (
	"FileName"	TEXT NOT NULL COLLATE NOCASE,
	"DirectoryName"	TEXT NOT NULL COLLATE NOCASE,
	"DetailsLastUpdated"	TEXT,
	"LastViewed"	TEXT,
	"Height"	INTEGER NOT NULL DEFAULT 0,
	"Width"	INTEGER NOT NULL DEFAULT 0,
	"FileSize"	INTEGER NOT NULL DEFAULT 0,
	"SoftDeleted"	INTEGER NOT NULL DEFAULT 0,
	"SoftDeletePending"	INTEGER NOT NULL DEFAULT 0,
	"NeedsToMove"	INTEGER NOT NULL DEFAULT 0,
	"HardDeletePending"	INTEGER NOT NULL DEFAULT 0,
	CONSTRAINT "PK_Files" PRIMARY KEY("FileName","DirectoryName")
);
CREATE INDEX IF NOT EXISTS "ixDirectoryAscFileDesc" ON "Files" (
	"DirectoryName"	ASC,
	"FileName"	DESC
);
CREATE INDEX IF NOT EXISTS "ixDirectoryDescFileDesc" ON "Files" (
	"DirectoryName"	DESC,
	"FileName"	DESC
);
CREATE INDEX IF NOT EXISTS "ixFileSizeAsc" ON "Files" (
	"FileSize"	ASC
);
CREATE INDEX IF NOT EXISTS "ixFileSizeDesc" ON "Files" (
	"FileSize"	DESC
);
CREATE INDEX IF NOT EXISTS "ixSoftDeletedAsc" ON "Files" (
	"SoftDeleted"	ASC
);
CREATE INDEX IF NOT EXISTS "ixSoftDeletedDesc" ON "Files" (
	"SoftDeleted"	DESC
);
CREATE INDEX IF NOT EXISTS "ixSoftDeletePendingAsc" ON "Files" (
	"SoftDeletePending"	ASC
);
CREATE INDEX IF NOT EXISTS "ixSoftDeletePendingDesc" ON "Files" (
	"SoftDeletePending"	DESC
);
CREATE INDEX IF NOT EXISTS "ixNeedsToMoveAsc" ON "Files" (
	"NeedsToMove"	ASC
);
CREATE INDEX IF NOT EXISTS "ixNeedsToMoveDesc" ON "Files" (
	"NeedsToMove"	DESC
);
CREATE INDEX IF NOT EXISTS "ixHardDeletePendingAsc" ON "Files" (
	"HardDeletePending"	ASC
);
CREATE INDEX IF NOT EXISTS "ixHardDeletePendingDesc" ON "Files" (
	"HardDeletePending"	DESC
);
COMMIT;
