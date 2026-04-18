create table if not exists user_profiles (
    id uuid primary key,
    external_identity_id varchar(200) not null unique,
    first_name varchar(100) not null,
    last_name varchar(100) not null,
    country varchar(100) not null,
    city varchar(100) not null,
    street varchar(150) not null,
    house varchar(50) not null,
    postal_code varchar(20) not null
);
